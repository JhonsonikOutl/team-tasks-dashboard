import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../../core/services/task.service';
import { ProjectService } from '../../core/services/project.service';
import { DeveloperService } from '../../core/services/developer.service';
import { Project } from '../../core/models/project.model';
import { Developer } from '../../core/models/developer.model';
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
  submitting = false;
  errorMessage = '';
  successMessage = '';

  statuses = [
    { id: 1, label: 'To Do' },
    { id: 2, label: 'In Progress' },
    { id: 3, label: 'Blocked' },
    { id: 4, label: 'Completed' }
  ];

  priorities = [
    { id: 1, label: 'Low' },
    { id: 2, label: 'Medium' },
    { id: 3, label: 'High' }
  ];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private projectService: ProjectService,
    private developerService: DeveloperService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      projectId: [null, Validators.required],
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', Validators.maxLength(1000)],
      assigneeId: [null],
      statusId: [1, Validators.required],
      priorityId: [1, Validators.required],
      estimatedComplexity: [3, [Validators.required, Validators.min(1), Validators.max(5)]],
      dueDate: ['', Validators.required]
    });

    this.projectService.getAll().subscribe(data => this.projects = data);
    this.developerService.getActive().subscribe(data => this.developers = data);
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