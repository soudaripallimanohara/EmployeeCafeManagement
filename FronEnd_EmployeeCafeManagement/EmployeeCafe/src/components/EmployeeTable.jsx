import { ModuleRegistry } from 'ag-grid-community';
import { ClientSideRowModelModule } from 'ag-grid-community';

// Register the required feature modules with the Grid
ModuleRegistry.registerModules([ClientSideRowModelModule]);

import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { Button } from '@mui/material';

export default function EmployeeTable({ employees, onEdit, onDelete }) {
  const columns = [
    { headerName: 'ID', field: 'id' },
    { headerName: 'Name', field: 'name' },
    { headerName: 'Email', field: 'emailAddress' },
    { headerName: 'Phone', field: 'phoneNumber' },
    { headerName: 'Days Worked', field: 'daysWorked' },
    { headerName: 'Café', field: 'cafe' },
    {
      headerName: 'Actions',
      cellRenderer: ({ data }) => (
        <>
          <Button onClick={() => onEdit(data)}>Edit</Button>
          <Button onClick={() => onDelete(data.id)} color="error">Delete</Button>
        </>
      )
    }
  ];

  return <div className="ag-theme-alpine" style={{ height: 400 }}><AgGridReact rowData={employees} columnDefs={columns} /></div>;
}