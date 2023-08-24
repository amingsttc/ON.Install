import React from "react";
import "@styles/CustomBulletItem.css";

type CustomBulletProps = {
  bullet: string;
  children;
};

export default function CustomBulletItem({
  bullet,
  children,
}: CustomBulletProps) {
  return (
    <li className="item">
      <span className="custom-bullet">{bullet}</span>
      {children}
    </li>
  );
}
