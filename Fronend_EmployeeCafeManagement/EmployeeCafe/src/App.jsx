import { Routes, Route } from "react-router-dom";
import CafesPage from "./pages/CafesPage";
import EmployeesPage from "./pages/EmployeesPage";
import Navbar from "./components/Navbar";

export default function App() {
  return (
    <div>
      <Navbar />
      <Routes>
        <Route path="/" element={<CafesPage />} />
        <Route path="/employees" element={<EmployeesPage />} />
      </Routes>
    </div>
  );
}
