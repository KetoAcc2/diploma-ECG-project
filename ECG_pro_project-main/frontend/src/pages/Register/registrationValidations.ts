const emailExpression: RegExp = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;
const passwordExpression: RegExp =
  /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=\S+$)(?=.*\W*)[0-9a-zA-Z\W]{8,}$/; //at least 8 chars, 1 number, 1 uppercase, 1 lowercase, 1 special
export type registrationValidationProp = {
  invalidEmail: boolean;
  invalidPassword: boolean;
};

const registrationValidation = (
  email: string,
  password: string
): registrationValidationProp => {
  return {
    invalidEmail: !validateEmail(email),
    invalidPassword: !validatePassword(password),
  };
};

export const validateEmail = (email: string): boolean => {
  return emailExpression.test(email);
};

export const validatePassword = (password: string): boolean => {
  return passwordExpression.test(password);
};

export default registrationValidation;
