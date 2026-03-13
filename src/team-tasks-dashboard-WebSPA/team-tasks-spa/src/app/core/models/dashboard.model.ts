export interface DeveloperWorkloadDto {
  developerId: number;
  developerName: string;
  openTasksCount: number;
  averageEstimatedComplexity: number;
}

export interface ProjectHealthDto {
  projectId: number;
  projectName: string;
  clientName: string;
  totalTasks: number;
  openTasks: number;
  completedTasks: number;
}

export interface DelayRiskDto {
  developerName: string;
  openTasksCount: number;
  avgDelayDays: number;
  nearestDueDate: string | null;
  latestDueDate: string | null;
  predictedCompletionDate: string | null;
  highRiskFlag: number;
}