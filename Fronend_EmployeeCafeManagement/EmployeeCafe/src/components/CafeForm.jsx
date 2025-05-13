import { useState, useEffect } from "react";
import ReusableTextField from "./ReusableTextField";
import { Button } from "@mui/material";
import { updateCafe } from "../api";
import { createCafe } from "../api";

export default function CafeForm({ editFlag, cafe = {}, onSubmit, onCancel }) {
  const [form, setForm] = useState({
    id: "0AB5321E-9D86-401F-A21B-16421FFFC058",
    name: "",
    description: "",
    logo: "",
    location: "",
    employeesCount: 0,
  });

  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (cafe && cafe.id) {
      setForm({
        id: cafe.id,
        name: cafe.name || "",
        description: cafe.description || "",
        logo: cafe.logo || "",
        location: cafe.location || "",
        employeesCount: cafe.employeesCount || 0,
      });
    }
  }, [cafe]);

  const handleChange = (e) =>
    setForm({ ...form, [e.target.name]: e.target.value });
  const validate = () => {
    const errs = {};
    if (!form.name) errs.name = "Name is required";
    if (!form.location) errs.location = "Location is required";
    if (form.description.length > 256) errs.description = "Max 256 chars";
    return errs;
  };

  const handleSubmit = async () => {
    console.log("Submitting form:", form);

    const errs = validate();
    if (Object.keys(errs).length) return setErrors(errs);

    try {
      if (editFlag) {
        await updateCafe(form); // PUT for update
        console.log("Cafe updated successfully");
      } else {
        await createCafe(form); // POST for create
        console.log("Cafe created successfully");
      }
      onSubmit?.(); // close modal and reload
    } catch (error) {
      console.error("Failed to submit cafe:", error);
    }
  };

  return (
    <>
      <ReusableTextField
        label="ID"
        name="id"
        value={form.id}
        onChange={handleChange}
        error={!!errors.id}
        helperText={errors.id}
        disabled
      />
      <ReusableTextField
        label="Name"
        name="name"
        value={form.name}
        onChange={handleChange}
        error={!!errors.name}
        helperText={errors.name}
      />
      <ReusableTextField
        label="Description"
        name="description"
        value={form.description}
        onChange={handleChange}
        error={!!errors.description}
        helperText={errors.description}
      />
      <ReusableTextField
        label="Location"
        name="location"
        value={form.location}
        onChange={handleChange}
      />
      <ReusableTextField
        label="Logo URL"
        name="logo"
        value={form.logo}
        onChange={handleChange}
      />
      <ReusableTextField
        label="Employees Count"
        name="employeesCount"
        type="number"
        value={form.employeesCount}
        disabled
        onChange={(e) =>
          handleChange({
            target: {
              name: e.target.name,
              value: parseInt(e.target.value, 10) || 0, // fallback to 0 or handle as needed
            },
          })
        }
      />

      <Button
        variant="contained"
        sx={{
          backgroundColor: "#1976d2",
          color: "#fff",
          borderRadius: "8px",
          padding: "10px 20px",
          "&:hover": {
            backgroundColor: "#1565c0",
          },
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          mt: 2,
          width: "100%",
        }}
        onClick={handleSubmit}
      >
        {editFlag ? "Update" : "Add"}
      </Button>
      <Button
        variant="contained"
        sx={{
          backgroundColor: "#1976d2",
          color: "fff",
          borderRadius: "8px",
          padding: "10px 20px",
          "&:hover": {
            backgroundColor: "#1565c5",
          },
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          mt: 2,
          width: "100%",
        }}
        onClick={onCancel}
      >
        Cancel
      </Button>
    </>
  );
}
1;
