import { useEffect, useState } from 'react';
import { fetchEmployees, fetchCafes, createEmployee, updateEmployee, deleteEmployee } from '../api';
import EmployeeTable from '../components/EmployeeTable';
import EmployeeForm from '../components/EmployeeForm';
import ConfirmDialog from '../components/ConfirmDialog';
import { Dialog } from '@mui/material';

export default function EmployeesPage() {
  const [employees, setEmployees] = useState([]);
  const [cafes, setCafes] = useState([]);
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [deleteId, setDeleteId] = useState(null);

  const loadData = async () => {
    const [empRes, cafeRes] = await Promise.all([fetchEmployees(), fetchCafes()]);
    setEmployees(empRes.data);
    setCafes(cafeRes.data);
  };

  useEffect(() => { loadData(); }, []);

  const handleEdit = (employee) => {
    setSelectedEmployee(employee);
    setDialogOpen(true);
  };

  const handleDelete = (id) => {
    setDeleteId(id);
    setConfirmOpen(true);
  };

  const confirmDelete = async () => {
    await deleteEmployee(deleteId);
    setConfirmOpen(false);
    loadData();
  };

  const handleSubmit = async (form) => {
    selectedEmployee ? await updateEmployee(selectedEmployee.id, form) : await createEmployee(form);
    setDialogOpen(false);
    setSelectedEmployee(null);
    loadData();
  };

  return (
    <div>
      <h2>Employees</h2>
      <button onClick={() => setDialogOpen(true)}>Add Employee</button>
      <EmployeeTable employees={employees} onEdit={handleEdit} onDelete={handleDelete} />

      <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)}>
        <EmployeeForm employee={selectedEmployee} cafes={cafes} onSubmit={handleSubmit} onCancel={() => setDialogOpen(false)} />
      </Dialog>

      <ConfirmDialog open={confirmOpen} onClose={() => setConfirmOpen(false)} onConfirm={confirmDelete} />
    </div>
  );
}
