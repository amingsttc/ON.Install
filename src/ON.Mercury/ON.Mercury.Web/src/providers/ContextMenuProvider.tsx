import React, { createContext, useContext, useState } from "react";

interface ContextMenuContextProps {
  showContextMenu: boolean;
  setShowContextMenu: React.Dispatch<React.SetStateAction<boolean>>;
}

const ContextMenuContext = createContext<ContextMenuContextProps | undefined>(
  undefined,
);

export function ContextMenuProvider({ children }) {
  const [showContextMenu, setShowContextMenu] = useState(false);

  const contextMenuValue: ContextMenuContextProps = {
    showContextMenu,
    setShowContextMenu,
  };

  return (
    <ContextMenuContext.Provider value={contextMenuValue}>
      {children}
    </ContextMenuContext.Provider>
  );
}

export function useContextMenu() {
  const context = useContext(ContextMenuContext);

  if (context === undefined) {
    throw new Error("useContextMenu must be used within a ContextMenuProvider");
  }

  return context;
}
