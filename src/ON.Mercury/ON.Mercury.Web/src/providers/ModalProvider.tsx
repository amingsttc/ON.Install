import React, { createContext, useContext, useState, ReactNode } from "react";
import "@styles/Modal.css";

interface ModalContextType {
  showModal: (content: ReactNode) => void;
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
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [modalContent, setModalContent] = useState<ReactNode | null>(null);

  const showModal = (content: ReactNode) => {
    setModalContent(content);
    setIsModalVisible(true);
  };

  const hideModal = () => {
    setModalContent(null);
    setIsModalVisible(false);
  };

  return (
    <ModalContext.Provider value={{ showModal, hideModal }}>
      {children}
      {isModalVisible && (
        <>
          <div className="modal-overlay" />
          <div className="modal">
            {modalContent}
            <button onClick={hideModal}>Close Modal</button>
          </div>
        </>
      )}
    </ModalContext.Provider>
  );
}
