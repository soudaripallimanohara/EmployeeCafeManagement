import { useEffect, useState } from "react";
import {
  fetchEmployees,
  fetchCafes,
  createEmployee,
  updateEmployee,
  deleteEmployee,
} from "../api";
import EmployeeTable from "../components/EmployeeTable";
import EmployeeForm from "../components/EmployeeForm";
import ConfirmDialog from "../components/ConfirmDialog";
import { Dialog } from "@mui/material";

export default function EmployeesPage() {
  const [employees, setEmployees] = useState([]);
  const [cafes, setCafes] = useState([]);
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [editFlag, setEditFlag] = useState(false);

  const loadData = async () => {
    const [empRes, cafeRes] = await Promise.all([
      fetchEmployees(),
      fetchCafes(),
    ]);
    setEmployees(empRes.data);
    setCafes(cafeRes.data);
  };

  useEffect(() => {
    loadData();
  }, []);

  const handleEdit = (employee) => {
    setEditFlag(true);
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
    setDialogOpen(false);
    setSelectedEmployee(null);
    loadData();
  };

  return (
    <div>
      <h2>Employees</h2>
      <button
        style={{
          backgroundColor: "lightblue",
          padding: "5px",
          fontSize: "18px",
          borderRadius: "10px",
          textShadow: "2px 2px 4px gray",
          fontWeight: "bold",
          cursor: "pointer",
        }}
        onClick={() => {
          setDialogOpen(true);
          setEditFlag(false);
        }}
      >
        Add Employee
      </button>
      <EmployeeTable
        employees={employees}
        onEdit={handleEdit}
        onDelete={handleDelete}
      />
      <Dialog
        open={dialogOpen}
        onClose={() => setDialogOpen(false)}
        fullWidth
        maxWidth="md"
        PaperProps={{
          sx: {
            width: "600px",
            backgroundColor: "#f9f9f9",
            borderRadius: 4,
            boxShadow: 10,
            p: 2,
          },
        }}
      >
        <EmployeeForm
          editFlag={editFlag}
          employee={selectedEmployee}
          cafes={cafes}
          onSubmit={handleSubmit}
          onCancel={() => {
            setDialogOpen(false);
            setSelectedEmployee(null);
          }}
        />
      </Dialog>

      <ConfirmDialog
        open={confirmOpen}
        onClose={() => setConfirmOpen(false)}
        onConfirm={confirmDelete}
      />
    </div>
  );
}
