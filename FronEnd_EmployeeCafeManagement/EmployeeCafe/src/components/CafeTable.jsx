import { ModuleRegistry } from 'ag-grid-community';
import { ClientSideRowModelModule } from 'ag-grid-community';

// Register the required feature modules with the Grid
ModuleRegistry.registerModules([ClientSideRowModelModule]);

import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { Button } from '@mui/material';

export default function CafeTable({ cafes, onEdit, onDelete, onViewEmployees }) {
  const columns = [
    { headerName: 'Logo', field: 'logo', cellRenderer: p => <img src={p.value} width={40} /> },
    { headerName: 'Cafe Id', field: 'id' },
    { headerName: 'Name', field: 'name' },
    { headerName: 'Description', field: 'description' },
    { headerName: 'Location', field: 'location' },
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

  return <div className="ag-theme-alpine" style={{ height: 400 }}><AgGridReact rowData={cafes} columnDefs={columns} /></div>;
}