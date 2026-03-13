import { Component, Input, Output, EventEmitter, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface TableColumn<T> {
  key: keyof T;
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
export class DatatableComponent<T extends object> {
  @Input() columns: TableColumn<T>[] = [];
  @Input() data: T[] = [];
  @Input() rowClass?: (row: T) => string;
  @Input() templates: { [key: string]: TemplateRef<any> } = {};
  @Output() rowClick = new EventEmitter<T>();

  sortKey: keyof T | '' = '';
  sortAsc: boolean = true;

  sort(key: keyof T): void {
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

  getRowClass(row: T): string {
    return this.rowClass ? this.rowClass(row) : '';
  }

  hasTemplate(key: string): boolean {
    return !!this.templates[key];
  }
}