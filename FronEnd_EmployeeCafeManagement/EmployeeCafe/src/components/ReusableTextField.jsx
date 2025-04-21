import { TextField } from '@mui/material';

export default function ReusableTextField({ label, value, onChange, error, helperText, ...props }) {
  return (
    <TextField
      label={label}
      value={value}
      onChange={onChange}
      error={error}
      helperText={helperText}
      fullWidth
      margin="normal"
      {...props}
    />
  );
}