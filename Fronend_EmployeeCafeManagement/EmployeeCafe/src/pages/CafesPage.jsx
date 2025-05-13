import { useEffect, useState } from "react";
import {
  fetchCafes,
  createCafe,
  updateCafe,
  deleteCafe,
  fetchEmployees,
} from "../api";
import CafeTable from "../components/CafeTable";
import CafeForm from "../components/CafeForm";
import ConfirmDialog from "../components/ConfirmDialog";
import { Dialog } from "@mui/material";
import EmployeeTable from "../components/EmployeeTable";
import Refresh from "../assets/refresh.png";

export default function CafesPage() {
  const [cafes, setCafes] = useState([]);
  const [selectedCafe, setSelectedCafe] = useState(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [viewEmployees, setViewEmployees] = useState(false);
  const [employees, setEmployees] = useState([]);
  const [editFlag, setEditFlag] = useState(false);

  const loadCafes = async () => {
    const res = await fetchCafes();
    //setCafes(res.data);
    setCafes(res.data.sort((a, b) => b.employeesCount - a.employeesCount));
  };

  useEffect(() => {
    loadCafes();
  }, []);

  const handleEdit = (cafe) => {
    setEditFlag(true);
    setSelectedCafe(cafe);
    setDialogOpen(true);
  };

  const handleDelete = (id) => {
    setDeleteId(id);
    setConfirmOpen(true);
  };

  const confirmDelete = async () => {
    await deleteCafe(deleteId);
    setConfirmOpen(false);
    loadCafes();
  };

  const handleSubmit = async (form) => {
    setDialogOpen(false);
    setSelectedCafe(null);
    loadCafes();
  };

  const handleViewEmployees = async (cafeId) => {
    const res = await fetchEmployees(cafeId);
    setEmployees(res.data);
    setViewEmployees(true);
  };

  return (
    <div>
      <h2>Cafés</h2>
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
        Add Café
      </button>
      <CafeTable
        cafes={cafes}
        onEdit={handleEdit}
        onDelete={handleDelete}
        onViewEmployees={handleViewEmployees}
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
        <CafeForm
          editFlag={editFlag}
          cafe={selectedCafe}
          onSubmit={handleSubmit}
          onCancel={() => {
            setDialogOpen(false);
            setSelectedCafe(null);
          }}
        />
      </Dialog>

      <ConfirmDialog
        open={confirmOpen}
        onClose={() => setConfirmOpen(false)}
        onConfirm={confirmDelete}
      />

      <Dialog
        open={viewEmployees}
        onClose={() => setViewEmployees(false)}
        maxWidth="md"
        fullWidth
      >
        <EmployeeTable
          employees={employees}
          //onEdit={() => {}}
          //onDelete={() => {}}
        />
      </Dialog>
    </div>
  );
}
