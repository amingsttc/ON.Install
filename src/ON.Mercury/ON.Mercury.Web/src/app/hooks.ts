import { useDispatch, useSelector } from "react-redux";
import type { TypedUseSelectorHook } from "react-redux";
import type { RootState, AppDispatch } from "./store";
import { useCallback } from "react";

export const useAppDispatch: () => AppDispatch = useDispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

export const useScrollLock = () => {
  const lockScroll = useCallback(() => {}, []);

  const unlockScroll = useCallback(() => {}, []);

  return {
    lockScroll,
    unlockScroll,
  };
};
