import React, { createContext, useContext, useState, ReactNode } from "react";
import "@styles/Modal.css";

interface ModalContextType {
  showModal: () => void;
  hideModal: () => void;
}

const ModalContext = createContext<ModalContextType | undefined>(undefined);

export function useModal() {
  const context = useContext(ModalContext);
  if (!context) {
    throw new Error("useModal must be used within a ModalProvider");
  }
  return context;
}

interface ModalProviderProps {
  children: ReactNode;
}

export function ModalProvider({ children }: ModalProviderProps) {
  const [isModalVisible, setIsModalVisible] = useState(true);

  const showModal = () => {
    setIsModalVisible(true);
  };

  const hideModal = () => {
    setIsModalVisible(false);
  };

  return (
    <ModalContext.Provider value={{ showModal, hideModal }}>
      {children}
      {isModalVisible && (
        <>
          <div className="modal-overlay" />
          <div className="modal">
            Modal Content
            <button onClick={hideModal}>Close Modal</button>
          </div>
        </>
      )}
    </ModalContext.Provider>
  );
}
