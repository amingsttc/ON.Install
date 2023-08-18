import React from "react";
import Sidebar from "../components/sidebar/sidebar";

type RootLayoutProps = {
  children;
};

function RootLayout({ children }: RootLayoutProps) {
  return (
    <>
      <Sidebar />
      {children}
    </>
  );
}

export default RootLayout;
