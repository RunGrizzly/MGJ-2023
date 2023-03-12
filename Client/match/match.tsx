import { Match, MatchData, Presence, Socket } from "@heroiclabs/nakama-js";
import { useState } from "react";
import { MatchMessage, MatchMessageType } from "../messages/matchMessage";
import {
  createSelectTrackMessage,
  TrackType,
} from "../messages/selectTrackMessage";

export const useMatch = (match: Match, socket: Socket, host: Presence) => {
  const [isAlive, setIsAlive] = useState(true);
  const [tileRequests, setTileRequests] = useState(0);
  const [isWinner, setIsWinner] = useState(false);
  const [name, setName] = useState("");

  socket.onmatchdata = receiveMatchState;

  function receiveMatchState(matchData: MatchData) {
    const jsonString = new TextDecoder().decode(matchData.data);
    const json = jsonString ? JSON.parse(jsonString) : "";
    console.log(
      `Received match data opCode ${matchData.op_code} ${JSON.stringify(json)} `
    );

    switch (matchData.op_code) {
      case MatchMessageType.StartGame:
        setName(json.name);
        break;
      case MatchMessageType.RequestTrack:
        requestTrack();
        break;
      default:
        break;
    }
  }

  function requestTrack() {
    setTileRequests((amount) => amount + 1);
  }

  async function onSelectTrack(trackId: TrackType) {
    const message = createSelectTrackMessage(trackId);
    setTileRequests((count) => count - 1);
    await sendMatchState(message);
  }

  async function sendMatchState(f: MatchMessage<unknown>) {
    console.log(
      `Sending match state ${f.type} ${JSON.stringify(f.data)} to host ${host}`
    );
    await socket.sendMatchState(
      match.match_id,
      f.type,
      JSON.stringify(f.data),
      [host]
    );
  }

  return {
    isAlive,
    isWinner,
    onSelectTrack,
    tileRequests,
    name,
  };
};
