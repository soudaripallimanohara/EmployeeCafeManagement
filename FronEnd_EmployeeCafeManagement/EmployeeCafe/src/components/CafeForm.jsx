import { useState, useEffect } from 'react';
import ReusableTextField from './ReusableTextField';
import { Button } from '@mui/material';
import { updateCafe } from '../api';

export default function CafeForm({ cafe = {}, onSubmit, onCancel }) {
  const [form, setForm] = useState({ id: '0AB5321E-9D86-401F-A21B-16421FFFC058',name: '', description: '', logo: '', location: '' });
  
  
  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (cafe && cafe.id) {
      setForm({
        id: cafe.id,
        name: cafe.name || '',
        description: cafe.description || '',
        logo: cafe.logo || '',
        location: cafe.location || ''
      });
    }
  }, [cafe]);
  

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });
  const validate = () => {
    const errs = {};
    if (!form.name) errs.name = 'Name is required';
    if (!form.location) errs.location = 'Location is required';
    if (form.description.length > 256) errs.description = 'Max 256 chars';
    return errs;
  };

  const handleSubmit = async () => {
    console.log('Submitting form:', form);
  
    const errs = validate();
    if (Object.keys(errs).length) return setErrors(errs);
  
    try {
      await updateCafe(form); // call api.js function
      console.log('Cafe updated successfully');
      onSubmit?.(); // optionally refresh parent or close modal
    } catch (error) {
      console.error('Failed to update cafe:', error);
    }
  };


  return (
    <>
      <ReusableTextField label="ID" name="id" value={form.id} onChange={handleChange} error={!!errors.id} helperText={errors.id} disabled  />
      <ReusableTextField label="Name" name="name" value={form.name} onChange={handleChange} error={!!errors.name} helperText={errors.name} />
      <ReusableTextField label="Description" name="description" value={form.description} onChange={handleChange} error={!!errors.description} helperText={errors.description} />
      <ReusableTextField label="Location" name="location" value={form.location} onChange={handleChange} />
      <ReusableTextField label="Logo URL" name="logo" value={form.logo} onChange={handleChange} />
      <Button onClick={handleSubmit}>Submit</Button>
      <Button onClick={onCancel}>Cancel</Button>
    </>
  );
}1