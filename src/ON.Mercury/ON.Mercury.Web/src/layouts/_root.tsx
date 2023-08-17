import React from "react";

type RootLayoutProps = {
  children;
};

function RootLayout({ children }: RootLayoutProps) {
  return (
    <>
      <h1>RootLayout</h1>
      {children}
    </>
  );
}

export default RootLayout;
