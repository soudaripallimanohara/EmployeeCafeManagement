import { Routes, Route } from 'react-router-dom';
import CafesPage from './pages/CafesPage';
import EmployeesPage from './pages/EmployeesPage';

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<CafesPage />} />
      <Route path="/employees" element={<EmployeesPage />} />
    </Routes>
  );
}
