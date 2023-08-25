import "@styles/ContextMenu.css";

import React, { useState } from "react";

export default function ContextMenu() {
  const [showContextMenu, setShowContextMenu] = useState(false);
  return (
    <>
      {showContextMenu && (
        <div id="context-menu" className="context-menu">
          <div
            className="context-menu-item"
            onClick={() => {
              setShowContextMenu(false);
            }}
          >
            Edit Message
          </div>
          <div className="context-menu-item">Delete Message</div>
        </div>
      )}
    </>
  );
}
