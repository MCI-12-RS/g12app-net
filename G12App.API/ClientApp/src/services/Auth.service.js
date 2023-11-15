
const Login = async (loginData) => {
  const response = await fetch('/api/auth/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(loginData)
  });

  const data = await response.json();
  return data;
};

export { Login };