import { useState, useEffect } from "react";
import ReusableTextField from "./ReusableTextField";
import { Button, MenuItem, TextField, Select } from "@mui/material";
import { isValidEmail, isValidPhone } from "../utils";
import { updateEmployee } from "../api";
import { createEmployee } from "../api";

export default function EmployeeForm({
  editFlag,
  employee = {},
  cafes = [],
  onSubmit,
  onCancel,
}) {
  const [form, setForm] = useState({
    id: "",
    name: "",
    emailAddress: "",
    phoneNumber: "",
    gender: "",
    cafeId: "",
    startDate: new Date().toISOString(),
  });

  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (employee && employee.id) {
      setForm({
        ...employee,
        startDate: employee.startDate || new Date().toISOString(),
      });
    }
  }, [employee]);

  useEffect(() => {
    if (cafes.length && !form.cafeId) {
      setForm((prev) => ({ ...prev, cafeId: cafes[0].id }));
    }
  }, [cafes]);

  const handleChange = (e) =>
    setForm({ ...form, [e.target.name]: e.target.value });

  const validate = () => {
    const errs = {};
    if (!form.id) errs.id = "Employee ID is required";
    if (!form.name) errs.name = "Name is required";
    if (!isValidEmail(form.emailAddress)) errs.emailAddress = "Invalid email";
    if (!isValidPhone(form.phoneNumber)) errs.phoneNumber = "Invalid phone";
    if (!form.gender) errs.gender = "Gender is required";
    if (!form.cafeId) errs.cafeId = "Cafe is required";
    return errs;
  };

  const handleSubmit = async () => {
    console.log("Submitting form:", form);

    const errs = validate();
    if (Object.keys(errs).length) return setErrors(errs);

    try {
      if (editFlag) {
        await updateEmployee(form); // PUT for update
        console.log("Employee updated successfully");
      } else {
        await createEmployee(form); // POST for create
        console.log("Employee created successfully");
      }

      onSubmit?.(); // close modal and reload
    } catch (error) {
      console.error("Failed to submit Employee:", error);
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
        label="Email"
        name="emailAddress"
        value={form.emailAddress}
        onChange={handleChange}
        error={!!errors.emailAddress}
        helperText={errors.emailAddress}
      />
      <ReusableTextField
        label="Phone"
        name="phoneNumber"
        value={form.phoneNumber}
        onChange={handleChange}
        error={!!errors.phoneNumber}
        helperText={errors.phoneNumber}
      />
      <ReusableTextField
        select
        label="Gender"
        name="gender"
        value={form.gender}
        onChange={handleChange}
        error={!!errors.gender}
        helperText={errors.gender}
        fullWidth
        margin="normal"
      >
        <MenuItem value="Male">Male</MenuItem>
        <MenuItem value="Female">Female</MenuItem>
      </ReusableTextField>

      <TextField
        select
        label="Café"
        name="cafeId"
        value={form.cafeId || ""}
        onChange={handleChange}
        fullWidth
        margin="normal"
      >
        {cafes.map((cafe) => (
          <MenuItem key={cafe.id} value={cafe.id}>
            {cafe.id}
          </MenuItem>
        ))}
      </TextField>

      <TextField
        label="Start Date"
        name="startDate"
        type="datetime-local"
        value={form.startDate.slice(0, 16)}
        error={!!errors.startDate}
        helperText={errors.startDate}
        onChange={(e) =>
          setForm({
            ...form,
            startDate: new Date(e.target.value).toISOString(),
          })
        }
        fullWidth
        margin="normal"
        inputProps={{
          max: new Date().toISOString().slice(0, 16), // 👈 max = now
        }}
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
