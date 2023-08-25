import React, { createContext, useContext, useState } from "react";

interface ContextMenuContextProps {
  showContextMenu: boolean;
  setShowContextMenu: React.Dispatch<React.SetStateAction<boolean>>;
  position: PositionInterface;
  setPosition: React.Dispatch<React.SetStateAction<PositionInterface>>;
}

interface PositionInterface {
  x: number;
  y: number;
}

const ContextMenuContext = createContext<ContextMenuContextProps | undefined>(
  undefined,
);

export function ContextMenuProvider({ children }) {
  const [showContextMenu, setShowContextMenu] = useState(false);
  const [position, setPosition] = useState<PositionInterface>({
    x: 0,
    y: 0,
  });

  const contextMenuValue: ContextMenuContextProps = {
    showContextMenu,
    setShowContextMenu,
    position,
    setPosition,
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
