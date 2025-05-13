import { ModuleRegistry } from "ag-grid-community";
import { ClientSideRowModelModule } from "ag-grid-community";

// Register the required feature modules with the Grid
ModuleRegistry.registerModules([ClientSideRowModelModule]);

import { AgGridReact } from "ag-grid-react";

import "ag-grid-community/styles/ag-grid.css";
import "ag-grid-community/styles/ag-theme-alpine.css";
import { Button } from "@mui/material";

export default function CafeTable({
  cafes,
  onEdit,
  onDelete,
  onViewEmployees,
}) {
  const columns = [
    {
      headerName: "Café Id",
      field: "id",
      editable: true,
      filter: "agTextColumnFilter",
    },
    {
      headerName: "Café Name",
      field: "name",
      editable: true,
      filter: "agTextColumnFilter",
    },
    {
      headerName: "Location",
      field: "location",
      editable: true,
      filter: "agTextColumnFilter",
    },
    {
      headerName: "Description",
      field: "description",
      editable: true,
      filter: "agTextColumnFilter",
    },
    {
      headerName: "Employee Count",
      field: "employeesCount",
      editable: true,
      filter: "agNumberColumnFilter",
      valueParser: (params) => Number(params.newValue),
    },

    {
      headerName: "Employees",
      field: "employeeView",
      cellRenderer: ({ data }) => (
        <Button onClick={() => onViewEmployees(data.id)}>View</Button>
      ),
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
        rowData={cafes}
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
