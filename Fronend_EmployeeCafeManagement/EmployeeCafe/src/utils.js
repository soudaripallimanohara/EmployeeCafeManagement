export const isValidEmail = (email) => /.+@.+\..+/.test(email);
export const isValidPhone = (phone) => /^[89]\d{7}$/.test(phone);