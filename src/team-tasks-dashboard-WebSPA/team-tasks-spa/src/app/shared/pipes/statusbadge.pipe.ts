import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusbadge',
  standalone: true
})
export class StatusbadgePipe implements PipeTransform {
  transform(value: string | null | undefined, type: 'status' | 'priority' = 'status'): string {

    if (!value) {
      return 'secondary';
    }

    if (type === 'status') {
      switch (value) {
        case 'To Do': return 'secondary';
        case 'In Progress': return 'primary';
        case 'Blocked': return 'danger';
        case 'Completed': return 'success';
        default: return 'secondary';
      }
    }

    if (type === 'priority') {
      switch (value) {
        case 'Low': return 'success';
        case 'Medium': return 'warning';
        case 'High': return 'danger';
        default: return 'secondary';
      }
    }

    return 'secondary';
  }
}