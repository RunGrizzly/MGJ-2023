import { MatchMessage } from "../messages/matchMessage";
import { MatchMessageType } from "./matchMessage";

export type TrackType = "LeftTurn" | "RightTurn" | "Straight";
export type SelectTrackMessage = MatchMessage<{
  trackId: TrackType;
}>;

export const createSelectTrackMessage = (
  trackId: TrackType
): SelectTrackMessage => ({
  type: MatchMessageType.TrackSelected,
  data: { trackId },
});
