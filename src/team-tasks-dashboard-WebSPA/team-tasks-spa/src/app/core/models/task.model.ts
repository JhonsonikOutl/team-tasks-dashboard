export interface Task {
  taskId: number;
  projectId: number;
  title: string;
  description: string;
  assigneeId: number | null;
  assigneeName: string;
  status: string;
  statusDisplay: string;
  priority: string;
  priorityDisplay: string;
  estimatedComplexity: number;
  dueDate: string;
  completionDate: string | null;
  createdAt: string;
  totalCount: number;
}

export interface CreateTask {
  projectId: number;
  title: string;
  description: string;
  assigneeId: number | null;
  statusId: number;
  priorityId: number;
  estimatedComplexity: number;
  dueDate: string;
}

export interface UpdateTaskStatus {
  statusId: number;
  priorityId?: number | null;
  estimatedComplexity?: number | null;
}

export interface TaskFilter {
  statusId?: number | null;
  assigneeId?: number | null;
  page: number;
  pageSize: number;
}