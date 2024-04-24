import {
  apiPath,
  fetchUrl,
  localStorageKeys,
  tokenStatus,
} from "../constants/constants";
import {
  ILoginDTO,
  IRegistrationDTO,
  UserRegisterDTO
} from "../interfaces/interface";
import { renewAccessToken, verifyTokenExpiration } from "./api";

/*
john@test.com
teacher123
*/

type ResetPasswordEmail = {
  email: string;
};
type ResetPasswordDTO = {
  password: string;
};

export const resetPassword = async (
  resetPasswordToken: string,
  password: string
) => {
  try {
    const response = fetch(
      `${apiPath.BASE_URL}${fetchUrl.RESET_SEND_EMAIL}/${resetPasswordToken}`,
      {
        method: "POST",
        body: JSON.stringify({
          password: password,
        } as ResetPasswordDTO),
        headers: {
          "Content-Type": "application/json",
        },
      }
    );
    return response;
  } catch (error) {
     
  }
};



export const resetPasswordSendEmail = async (email: string) => {
  try {
    const response = await fetch(
      `${apiPath.BASE_URL}${fetchUrl.RESET_SEND_EMAIL}`,
      {
        method: "POST",
        body: JSON.stringify({
          email: email,
        } as ResetPasswordEmail),
        headers: {
          "Content-Type": "application/json",
        },
      }
    );
    return response;
  } catch (error) {
  }
};

export const activateAccount = async (activationToken: string) => {
  try {
    const response = fetch(
      `${apiPath.BASE_URL}${fetchUrl.ACTIVATE}/${activationToken}`,
      {
        method: "POST",
      }
    );
    return response;
  } catch (error) {
     
  }
};

export const register = async (registerCredentials: IRegistrationDTO) => {
  try {
    const response = await fetch(`${apiPath.BASE_URL}${fetchUrl.REGISTER}`, {
      method: "POST",
      body: JSON.stringify({
        email: registerCredentials.email,
        password: registerCredentials.password,
      } as UserRegisterDTO),
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response;
  } catch (error) {
     
  }
};

export const logIn = async (credentials: ILoginDTO) => {
  try {
    const response = await fetch(`${apiPath.BASE_URL}${fetchUrl.LOGIN}`, {
      method: "POST",
      body: JSON.stringify(credentials),
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response;
  } catch (error) {
     
  }
};

export const logOut = async () => {
  const status = verifyTokenExpiration();
  if (status !== tokenStatus.VALID) {
    await renewAccessToken();
  }
  const userData = localStorage.getItem(localStorageKeys.USER_PERSISTANCE);
  if (userData === null || userData === undefined) {
    return null;
  }
  const parsed = JSON.parse(userData!);
  const email = parsed["userData"]["email"];
  const jwt = parsed["jwtToken"]["accessToken"];
  const response = await fetch(
    `${apiPath.BASE_URL}/Auth/Logout?email=${email}`,
    {
      method: "POST",
      headers: {
        "Access-Control-Allow-Credentials": "true",
        Authorization: `Bearer ${jwt}`,
        "Content-Type": "application/json",
      },
    }
  );

  if (response.status === 200) {
    localStorage.removeItem(localStorageKeys.USER_PERSISTANCE);
  }

  return response;
};
