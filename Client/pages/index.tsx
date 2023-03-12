import Head from "next/head";
import { Client, Match, Presence, Session } from "@heroiclabs/nakama-js";
import { useEffect, useState } from "react";
import { useInterval } from "usehooks-ts";
import { useMatch } from "../match/match";

const getState = (client: ReturnType<typeof useClient>) => {
  if (!client.match && !client.ticket) {
    return "home";
  } else if (!client.match && client.ticket) {
    return "waiting";
  } else if (client.match) {
    return "ingame";
  } else {
    return "confused";
  }
};

export default function Home() {
  const client = useClient();
  const state = getState(client);

  const Join = () => (
    <button
      className="button"
      disabled={!client.connected}
      onClick={() => client.join()}
    >
      Join
    </button>
  );
  const Waiting = () => <h2 className="waiting">Waiting for match!</h2>;

  return (
    <>
      <h1 className="title">RAIL RUMBLE</h1>
      <div className={"container"}>
        <Head>
          <title>RAIL RUMBLE</title>
          <link rel="icon" href="/favicon.ico" />
        </Head>

        {state === "home" && <Join />}
        {state === "waiting" && <Waiting />}
        {state === "ingame" && <InGame client={client} />}
        {state === "confused" && <h2>SEND HELP</h2>}
      </div>
    </>
  );
}

type Props = { client: ReturnType<typeof useClient> };
const InGame = ({ client }: Props) => {
  const match = useMatch(client.match, socket, client.host);

  return (
    <>
      <h2 className="name">{match.name}</h2>
      <div className="button-row">
        <button
          disabled={match.tileRequests === 0}
          onClick={() => match.onSelectTrack("LeftTurn")}
        >
          LEFT
        </button>
        <button
          disabled={match.tileRequests === 0}
          onClick={() => match.onSelectTrack("Straight")}
        >
          STRAIGHT
        </button>
        <button
          disabled={match.tileRequests === 0}
          onClick={() => match.onSelectTrack("RightTurn")}
        >
          RIGHT
        </button>
      </div>
    </>
  );
};

const client = new Client("defaultkey", "172.25.120.251", "7350");
const socket = client.createSocket();

const useClient = () => {
  const [session, setSession] = useState<Session>(null);
  const [connected, setIsConnected] = useState(false);
  const [isConnecting, setIsConnecting] = useState(false);
  const [isMatchInProgress, setMatchInProgress] = useState(true);
  const [match, setMatch] = useState<Match | null>(null);
  const [host, setHost] = useState<Presence>();
  const [ticket, setTicket] = useState("");

  useEffect(() => {
    function onDisconnect(e: Event) {
      setIsConnected(false);
    }
    socket.onerror = (error) => {
      console.error(`Received socket error`, error);
    };
    socket.ondisconnect = onDisconnect;
  }, []);

  useInterval(
    async () => {
      const matches = await client.listMatches(session);
      if (matches.matches?.length > 0) {
        setMatchInProgress(true);
      } else {
        setMatchInProgress(false);
      }
    },
    match ? null : 1000
  );

  async function connect() {
    setIsConnecting(true);
    await socket.connect(session, false);
    setIsConnected(true);
    setIsConnecting(false);
  }

  useEffect(() => {
    if (session && !isConnecting) {
      connect();
    }
  }, [session]);

  async function join() {
    socket.onmatchmakermatched = async (matched) => {
      console.log("GOT MATCH MADE, JOINING MATCH");
      const match = await socket.joinMatch(matched.match_id, matched.token);
      console.log(`JOINED MATCH ${match.match_id} ${JSON.stringify(match)}`);
      setMatch(match);
      setHost(
        matched.users.find((u) => u.string_properties?.type === "host").presence
      );
    };
    console.log("Searching for match");
    const ticket = await socket.addMatchmaker("+properties.type:host", 2, 3);
    setTicket(ticket.ticket);
  }

  useEffect(() => {
    async function login() {
      const account = await client.authenticateDevice(
        "clientclientclient",
        true,
        "client"
      );
      setSession(account);
    }

    login();
  }, []);

  return {
    socket,
    connected,
    session,
    connect,
    join,
    isMatchInProgress,
    match,
    host,
    ticket,
  };
};
