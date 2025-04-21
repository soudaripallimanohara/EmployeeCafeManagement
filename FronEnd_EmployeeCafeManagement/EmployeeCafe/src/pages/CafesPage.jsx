import { useEffect, useState } from 'react';
import { fetchCafes, createCafe, updateCafe, deleteCafe, fetchEmployees } from '../api';
import CafeTable from '../components/CafeTable';
import CafeForm from '../components/CafeForm';
import ConfirmDialog from '../components/ConfirmDialog';
import { Dialog } from '@mui/material';
import EmployeeTable from '../components/EmployeeTable';

export default function CafesPage() {
  const [cafes, setCafes] = useState([]);
  const [selectedCafe, setSelectedCafe] = useState(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [viewEmployees, setViewEmployees] = useState(false);
  const [employees, setEmployees] = useState([]);

  const loadCafes = async () => {
    const res = await fetchCafes();
    setCafes(res.data);
  };

  useEffect(() => { loadCafes(); }, []);

  const handleEdit = (cafe) => {
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
    selectedCafe ? await updateCafe(selectedCafe.id, form) : await createCafe(form);
    setDialogOpen(false);
    setSelectedCafe(null);
    loadCafes();
  }; 

/*   const handleEditSubmitSuccess = () => {
    setIsEditModalOpen(false);
    fetchCafes(); // refresh grid or whatever
  }; */

  const handleViewEmployees = async (cafeId) => {
    const res = await fetchEmployees(cafeId);
    setEmployees(res.data);
    setViewEmployees(true);
  };

  return (
    <div>
      <h2>Cafés</h2>
      <button onClick={() => setDialogOpen(true)}>Add Café</button>
      <CafeTable cafes={cafes} onEdit={handleEdit} onDelete={handleDelete} onViewEmployees={handleViewEmployees} />
      
      <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)}>
        <CafeForm cafe={selectedCafe} onSubmit={handleSubmit} onCancel={() => setDialogOpen(false)} />

      </Dialog>

      <ConfirmDialog open={confirmOpen} onClose={() => setConfirmOpen(false)} onConfirm={confirmDelete} />

      <Dialog open={viewEmployees} onClose={() => setViewEmployees(false)} maxWidth="md" fullWidth>
        <EmployeeTable employees={employees} onEdit={() => {}} onDelete={() => {}} />
      </Dialog>
    </div>
  );
}
