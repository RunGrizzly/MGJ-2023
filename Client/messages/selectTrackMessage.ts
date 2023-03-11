import { MatchMessage } from "../messages/matchMessage";
import { MatchMessageType } from "./matchMessage";

export type SelectTrackMessage = MatchMessage<{
  trackId: number;
}>;

export const createSelectTrackMessage = (
  trackId: number
): SelectTrackMessage => ({
  type: MatchMessageType.TrackSelected,
  data: { trackId },
});
