import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';
import { Chart, ChartData, ChartOptions } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { ProjectService } from '../../core/services/project.service';
import { DeveloperService } from '../../core/services/developer.service';
import { Project } from '../../core/models/project.model';
import { Task } from '../../core/models/task.model';
import { Developer } from '../../core/models/developer.model';
import { DatatableComponent, TableColumn } from '../../shared/components/datatable/datatable.component';
import { StatusbadgePipe } from '../../shared/pipes/statusbadge.pipe';
import { TaskFormComponent } from '../task-form/task-form.component';

Chart.register(ChartDataLabels);

@Component({
  selector: 'app-project-tasks',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, NgChartsModule, DatatableComponent, StatusbadgePipe, TaskFormComponent],
  templateUrl: './project-tasks.component.html',
  styleUrl: './project-tasks.component.scss'
})
export class ProjectTasksComponent implements OnInit {
  project: Project | null = null;
  tasks: Task[] = [];
  developers: Developer[] = [];
  selectedTask: Task | null = null;
  showTaskForm = false;

  filterStatusId: number | null = null;
  filterAssigneeId: number | null = null;
  page = 1;
  pageSize = 10;
  totalCount = 0;

  get hasNextPage(): boolean {
    return this.page * this.pageSize < this.totalCount;
  }

  statuses = [
    { id: 1, label: 'To Do' },
    { id: 2, label: 'In Progress' },
    { id: 3, label: 'Blocked' },
    { id: 4, label: 'Completed' }
  ];

  @ViewChild('statusTpl',   { static: true }) statusTpl!:   TemplateRef<any>;
  @ViewChild('priorityTpl', { static: true }) priorityTpl!: TemplateRef<any>;

  columns: TableColumn[] = [];
  templates: { [key: string]: TemplateRef<any> } = {};

  chartData: ChartData<'doughnut'> = {
    labels: ['To Do', 'In Progress', 'Blocked', 'Completed'],
    datasets: [{
      data: [0, 0, 0, 0],
      backgroundColor: ['#6c757d', '#4f8ef7', '#dc3545', '#198754'],
      borderWidth: 0
    }]
  };

  chartOptions: ChartOptions<'doughnut'> = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'right',
        labels: { color: '#e6edf3', padding: 16, font: { size: 13 } }
      },
      datalabels: {
        color: '#fff',
        font: { size: 13, weight: 'bold' },
        formatter: (value: number) => value > 0 ? value : ''
      }
    }
  };

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectService,
    private developerService: DeveloperService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.columns = [
      { key: 'title', label: 'Título', sortable: true},
      { key: 'assigneeName', label: 'Asignado a'},
      { key: 'status', label: 'Estado', template: 'status'},
      { key: 'priority', label: 'Prioridad', template: 'priority'},
      { key: 'estimatedComplexity', label: 'Complejidad' },
      { key: 'createdAt', label: 'Creada'},
      { key: 'dueDate', label: 'Vencimiento'}
    ];

    this.templates = {
      status:   this.statusTpl,
      priority: this.priorityTpl
    };

    this.projectService.getById(id).subscribe(p => this.project = p);
    this.developerService.getActive().subscribe(d => this.developers = d);
    this.loadTasks();
  }

  loadTasks(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.projectService
      .getTasksByProject(id, this.filterStatusId, this.filterAssigneeId, this.page, this.pageSize)
      .subscribe(data => {
        this.tasks = data;
        this.totalCount = data.length > 0 ? data[0].totalCount : 0;
        this.updateChart(data);
      });
  }

  updateChart(tasks: Task[]): void {
    const counts = [0, 0, 0, 0];
    tasks.forEach(t => {
      if (t.status === 'To Do')            counts[0]++;
      else if (t.status === 'In Progress') counts[1]++;
      else if (t.status === 'Blocked')     counts[2]++;
      else if (t.status === 'Completed')   counts[3]++;
    });
    this.chartData = {
      ...this.chartData,
      datasets: [{ ...this.chartData.datasets[0], data: counts }]
    };
  }

  applyFilters(): void { this.page = 1; this.loadTasks(); }
  prevPage(): void { if (this.page > 1) { this.page--; this.loadTasks(); } }
  nextPage(): void { if (this.hasNextPage) { this.page++; this.loadTasks(); } }
  onTaskClick(task: Task): void { this.selectedTask = task; }
  closeDetail(): void { this.selectedTask = null; }
  onTaskCreated(): void { this.showTaskForm = false; this.loadTasks(); }
}