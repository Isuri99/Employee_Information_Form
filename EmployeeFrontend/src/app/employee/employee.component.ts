import { Component, OnInit } from '@angular/core';
import { Employee } from '../employee.model';
import { EmployeeService } from '../employee.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';



@Component({
  selector: 'app-employee',
  standalone: true,    
  imports: [CommonModule, ReactiveFormsModule], 
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})

export class EmployeeComponent implements OnInit {
  employees: Employee[] = [];
  averageSalary: number = 0;
  selectedRow: Employee | null = null;

  
  employeeForm!: FormGroup;

  constructor(private empService: EmployeeService) {}

  ngOnInit(): void {
    this.initForm();
    this.refreshData();
  }

  
  initForm() {
    this.employeeForm = new FormGroup({
      employeeNumber: new FormControl(null, [Validators.required]),
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      dateOfBirth: new FormControl('', Validators.required),
      salary: new FormControl(null, [Validators.required, Validators.min(0)])
    });
  }

  refreshData() {
    this.empService.getEmployees().subscribe(data => this.employees = data);
    this.empService.getAverageSalary().subscribe(avg => this.averageSalary = avg);
  }

  onSelectRow(emp: Employee) {
    this.selectedRow = emp;
  }

  onLoadDataToForm() {
    if (this.selectedRow) {
      const formattedDate = this.selectedRow.dateOfBirth.split('T')[0];
     
      this.employeeForm.patchValue({
        ...this.selectedRow,
        dateOfBirth: formattedDate
      });
    } else {
      alert("Please select a row first!");
    }
  }

  onSave() {
    if (this.employeeForm.valid) {
      this.empService.saveEmployee(this.employeeForm.value).subscribe(() => {
        this.refreshData();
        this.onCancel();
      });
    }
  }

  onUpdate() {
    if (this.employeeForm.valid) {
      this.empService.saveEmployee(this.employeeForm.value).subscribe({
        next: () => {
          alert("Employee Updated successfully!");
          this.refreshData();
          this.onCancel();
        },
        error: (err) => console.error(err)
      });
    }
  }

  onDelete(id: number) {
    if(!id) return alert("Select an employee to delete");
    if(confirm('Are you sure?')) {
      this.empService.deleteEmployee(id).subscribe(() => {
        this.refreshData();
        this.onCancel();
      });
    }
  }

  onCancel() {
    this.employeeForm.reset();
    this.selectedRow = null;
  }
}