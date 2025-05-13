import { ModuleRegistry } from "ag-grid-community";
import { ClientSideRowModelModule } from "ag-grid-community";

// Register the required feature modules with the Grid
ModuleRegistry.registerModules([ClientSideRowModelModule]);

import { AgGridReact } from "ag-grid-react";
import "ag-grid-community/styles/ag-grid.css";
import "ag-grid-community/styles/ag-theme-alpine.css";
import { Button } from "@mui/material";

export default function EmployeeTable({ employees, onEdit, onDelete }) {
  const columns = [
    {
      headerName: "ID",
      field: "id",
      editable: true,
      filter: "agTextColumnFilter",
    },
    {
      headerName: "Name",
      field: "name",
      editable: true,
      filter: "agTextColumnFilter",
    },
    {
      headerName: "Email",
      field: "emailAddress",
      editable: true,
      filter: "agTextColumnFilter",
    },
    {
      headerName: "Phone",
      field: "phoneNumber",
      editable: true,
      filter: "agTextColumnFilter",
    },
    {
      headerName: "Days worked",
      field: "daysWorked",
      editable: true,
      filter: "agTextColumnFilter",
    },
    {
      headerName: "Cafe",
      field: "cafe",
      editable: true,
      filter: "agTextColumnFilter",
    },
    {
      headerName: "Actions",
      cellRenderer: (params) => (
        <div>
          <button
            style={{ backgroundColor: "lightblue" }}
            onClick={() => onEdit(params.data)}
          >
            Edit
          </button>
          <button
            style={{ backgroundColor: "lightblue" }}
            onClick={() => onDelete(params.data.id)}
          >
            Delete
          </button>
        </div>
      ),
      editable: false,
      sortable: false,
      filter: false,
    },
  ];

  return (
    <div className="ag-theme-alpine" style={{ height: 400 }}>
      <AgGridReact
        rowData={employees}
        columnDefs={columns}
        defaultColDef={{
          sortable: true,
          filter: true,
          resizable: true,
          flex: 1,
        }}
        animateRows={true}
        domLayout="autoHeight"
        pagination={true}
        paginationPageSize={10}
      />
    </div>
  );
}
