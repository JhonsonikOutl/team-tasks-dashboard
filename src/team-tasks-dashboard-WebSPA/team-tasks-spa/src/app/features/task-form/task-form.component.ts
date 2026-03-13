import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../../core/services/task.service';
import { ProjectService } from '../../core/services/project.service';
import { DeveloperService } from '../../core/services/developer.service';
import { CatalogService } from '../../core/services/catalog.service';
import { Project } from '../../core/models/project.model';
import { Developer } from '../../core/models/developer.model';
import { CatalogItem } from '../../core/models/catalog.model';
import { CreateTask } from '../../core/models/task.model';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.component.html',
  styleUrl: './task-form.component.scss'
})
export class TaskFormComponent implements OnInit {
  @Output() close = new EventEmitter<void>();
  @Output() taskCreated = new EventEmitter<void>();

  form!: FormGroup;
  projects: Project[] = [];
  developers: Developer[] = [];
  statuses: CatalogItem[] = [];
  priorities: CatalogItem[] = [];
  submitting = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private projectService: ProjectService,
    private developerService: DeveloperService,
    private catalogService: CatalogService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      projectId: [null, Validators.required],
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', Validators.maxLength(1000)],
      assigneeId: [null],
      statusId: [null, Validators.required],
      priorityId: [null, Validators.required],
      estimatedComplexity: [3, [Validators.required, Validators.min(1), Validators.max(5)]],
      dueDate: ['', Validators.required]
    });

    this.projectService.getAll().subscribe(data => this.projects = data);
    this.developerService.getActive().subscribe(data => this.developers = data);

    this.catalogService.getTaskStatuses().subscribe(data => {
      this.statuses = data;
      this.form.patchValue({ statusId: data[0]?.id ?? null });
    });

    this.catalogService.getTaskPriorities().subscribe(data => {
      this.priorities = data;
      this.form.patchValue({ priorityId: data[0]?.id ?? null });
    });
  }

  get f() { return this.form.controls; }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting = true;
    this.errorMessage = '';

    const dto: CreateTask = this.form.value;

    this.taskService.create(dto).subscribe({
      next: () => {
        this.successMessage = 'Tarea creada exitosamente.';
        this.submitting = false;
        setTimeout(() => this.taskCreated.emit(), 1000);
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Error al crear la tarea.';
        this.submitting = false;
      }
    });
  }
}