import { useState, useEffect } from 'react';
import ReusableTextField from './ReusableTextField';
import { Button, MenuItem, TextField, Select } from '@mui/material';
import { isValidEmail, isValidPhone } from '../utils';
import { updateEmployee } from '../api';

export default function EmployeeForm({ employee = {}, cafes = [], onSubmit, onCancel }) {
  const [form, setForm] = useState({
     id: '',
     name: '',
     emailAddress: '',
     phoneNumber: '',
     gender: '',
     cafeId: '',
     startDate: new Date().toISOString()
  });

  const [errors, setErrors] = useState({});

     useEffect(() => {
    if (employee && employee.id) {
      setForm({ ...employee, startDate: employee.startDate || new Date().toISOString() });
    }
  }, [employee]); 
  
  useEffect(() => {
    if (cafes.length && !form.cafeId) {
      setForm((prev) => ({ ...prev, cafeId: cafes[0].id }));
    }
  }, [cafes]);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const validate = () => {
    const errs = {};
    if (!form.id) errs.id = 'Employee ID is required';
    if (!form.name) errs.name = 'Name is required';
    if (!isValidEmail(form.emailAddress)) errs.emailAddress = 'Invalid email';
    if (!isValidPhone(form.phoneNumber)) errs.phoneNumber = 'Invalid phone';
    if (!form.gender) errs.gender = 'Gender is required';
    if (!form.cafeId) errs.cafeId = 'Cafe is required';
    return errs;
  };

     const handleSubmit = () => {
     console.log('Submitting form:', form);
     const errs = validate();
     if (Object.keys(errs).length) return setErrors(errs);
     onSubmit(form);
     console.log('Employee added successfully');
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
      <TextField
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
       </TextField>

      <TextField
        select
        label="Café"
        name="cafeId"
        value={form.cafeId || ''}
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
        onChange={(e) => setForm({ ...form, startDate: new Date(e.target.value).toISOString() })}
        fullWidth
        margin="normal"
      />
      <Button onClick={handleSubmit} variant="contained">Submit</Button>
      <Button onClick={onCancel} style={{ marginLeft: 8 }}>Cancel</Button>
    </>
  );
}
