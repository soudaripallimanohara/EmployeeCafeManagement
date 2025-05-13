import axios from "axios";

/* const api = axios.create({
  baseURL: "https://localhost:7137/api", // Adjust to your backend URL
}); */

const api = axios.create({
  baseURL: "https://localhost:7137/api",
  headers: {
    "Content-Type": "application/json",
  },
});

//export const fetchCafes = () => api.get("/Cafe");
//export const createCafe = (data) => api.post("/Cafe", data);
//export const updateCafe = (data) => api.put(`/Cafe`, data);
//export const createEmployee = (data) => api.post("/Employee", data);
//export const updateEmployee = (data) => api.put("/Employee", data);

export const fetchCafes = () => {
  return api.get("/Cafe"); // ✅ Let Axios handle headers
};

/* export const createCafe = (data) => {
  const formData = new FormData();
  for (const key in data) {
    if (data[key] !== undefined && data[key] !== null) {
      formData.append(key, data[key]);
    }
  }

  return api.post("/Cafe", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
};
 */
/* export const createCafe = (data) =>
  api.post("/Cafe", data, {
    headers: { "Content-Type": "application/json" },
  }); */

export const createCafe = async (data) => {
  try {
    const response = await api.post("/Cafe", JSON.stringify(data), {
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error response:", error.response?.data || error.message);
    throw error;
  }
};

/* export async function createCafe(cafe) {
  const url = "https://localhost:7137/api/Cafe";
  const response = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    // ✅ Wrap cafe object in cafeDto
    body: JSON.stringify({ cafeDto: cafe }),
  });

  if (!response.ok) {
    const errorData = await response.json();
    console.error("Error response:", errorData);
    throw new Error(`HTTP ${response.status}`);
  }

  return await response.json();
} */

export const updateCafe = (data) =>
  api.put("/Cafe", data, {
    headers: { "Content-Type": "application/json" },
  });

export const deleteCafe = (id) => api.delete(`/Cafe?id=${id}`);

/* export const fetchEmployees = (cafe) =>
  api.get("/Employee", { params: { cafe } }); */

export const fetchEmployees = (cafeId) =>
  api.get("/Employee", { params: { cafeId } });

export const createEmployee = (data) =>
  api.post("/Employee", data, {
    headers: { "Content-Type": "application/json" },
  });

export const updateEmployee = (data) =>
  api.put("/Employee", data, {
    headers: { "Content-Type": "application/json" },
  });

export const deleteEmployee = (id) => api.delete(`/Employee?id=${id}`);
