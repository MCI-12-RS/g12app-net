import React, { useContext, useEffect } from 'react';
import Toast from 'react-bootstrap/Toast';
import ToastContainer from 'react-bootstrap/ToastContainer';

const ToastContext = React.createContext();
export const useToastContext = () => useContext(ToastContext);

const { Provider, Consumer } = ToastContext;

const ToastContextProvider = ({ children }) => {
  const [toasts, setToasts] = React.useState([]);

  const addToast = (toast) => {
    toast.id = toast.id ?? parseInt(Math.random()*100000);
    setToasts([...toasts, toast]);
  };

  const removeToast = (toast) => {
    setToasts(toasts.filter((t) => t.id !== toast.id));
  };

  return (
    <Provider value={{ addToast, removeToast }}>
      {children}
      <ToastContainer
          className="p-3"
          position="bottom-end"          
          style={{ zIndex: 1 }}
        >
      {toasts.map((toast) => (
        <Toast
        className="m-1"
        key={toast.id + toast.title}
        show={toasts.some(x => x.id == toast.id)}
        delay={toast.hideDelay !== undefined && toast.hideDelay} autohide={toast.hideDelay !== undefined}
        bg={toast.type}
        onClose={() => removeToast(toast)}
      >
        <Toast.Header>
          <strong className="me-auto">{toast.title}</strong>
        </Toast.Header>
        <Toast.Body className={toast.type === 'dark' && 'text-white'}>
          {toast.description}
        </Toast.Body>
      </Toast>
      ))}
      </ToastContainer>
    </Provider>
  );
}

export { ToastContextProvider, Consumer as ConsumerProvider };
export default ToastContext;