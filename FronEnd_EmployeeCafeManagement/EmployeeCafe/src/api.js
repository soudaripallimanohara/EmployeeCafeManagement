import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:5001/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

export const fetchCafes = () => api.get('/Cafe');
export const createCafe = (data) => api.post('/Cafe', data);
export const updateCafe = (data) => api.put(`/Cafe`, data);
export const deleteCafe = (id) => api.delete(`/Cafe?id=${id}`);

export const fetchEmployees = (cafe) => api.get('/Employee', { params: { cafe } });
export const createEmployee = (data) => api.post('/Employee', data);

 export const updateEmployee = (employee) => {
  // Clean up the object if needed
  const cleanEmployee = {
    id: employee.id,
    name: employee.name,
    emailAddress: employee.emailAddress,
    phoneNumber: employee.phoneNumber,
    gender: employee.gender,
    cafeId: employee.cafeId,
    startDate: employee.startDate, // should be full ISO string
  };

  //return axios.put(`https://localhost:5001/api/Employee/${employee.id}`, cleanEmployee);
  //return axios.put(`https://localhost:5001/api/Employee`, cleanEmployee);
  return axios.put('https://localhost:5001/api/Employee', cleanEmployee);
}; 




export const deleteEmployee = (id) => api.delete(`/Employee?id=${id}`);