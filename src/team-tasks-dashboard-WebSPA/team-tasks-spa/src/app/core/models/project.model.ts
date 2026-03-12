export interface Project {
  projectId: number;
  name: string;
  clientName: string;
  status: string;
  totalTasks: number;
  openTasks: number;
  completedTasks: number;
}