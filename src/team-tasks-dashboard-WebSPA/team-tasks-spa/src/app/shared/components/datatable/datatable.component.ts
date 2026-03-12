import { Component, Input, Output, EventEmitter, ContentChildren, QueryList, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface TableColumn {
  key: string;
  label: string;
  sortable?: boolean;
  template?: string;
}

@Component({
  selector: 'app-datatable',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './datatable.component.html',
  styleUrl: './datatable.component.scss'
})
export class DatatableComponent {
  @Input() columns: TableColumn[] = [];
  @Input() data: any[] = [];
  @Input() rowClass?: (row: any) => string;
  @Input() templates: { [key: string]: TemplateRef<any> } = {};
  @Output() rowClick = new EventEmitter<any>();

  sortKey: string = '';
  sortAsc: boolean = true;

  sort(key: string): void {
    if (this.sortKey === key) {
      this.sortAsc = !this.sortAsc;
    } else {
      this.sortKey = key;
      this.sortAsc = true;
    }
    this.data = [...this.data].sort((a, b) => {
      const valA = a[key];
      const valB = b[key];
      if (valA < valB) return this.sortAsc ? -1 : 1;
      if (valA > valB) return this.sortAsc ? 1 : -1;
      return 0;
    });
  }

  getRowClass(row: any): string {
    return this.rowClass ? this.rowClass(row) : '';
  }

  hasTemplate(key: string): boolean {
    return !!this.templates[key];
  }
}