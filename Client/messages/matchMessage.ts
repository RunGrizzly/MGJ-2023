type ValueOf<T> = T[keyof T];
export type MatchMessageType = ValueOf<typeof MatchMessageType>;
export const MatchMessageType = {
  StartGame: 1,
  RequestTrack: 2,
  TrackSelected: 3,
} as const;
export type MatchMessage<T> = { type: MatchMessageType; data?: T };
