import Head from "next/head";
import styles from "../styles/Home.module.css";
import { Client, Match, Presence, Session } from "@heroiclabs/nakama-js";
import { useEffect, useState } from "react";
import { useInterval } from "usehooks-ts";
import { useMatch } from "../match/match";

export default function Home() {
  const client = useClient();

  return (
    <div className={styles.container}>
      <Head>
        <title>Create Next App</title>
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main>
        <h1>TRONS</h1>
        <button disabled={!client.connected} onClick={() => client.join()}>
          Join
        </button>
        <p>
          {client.isMatchInProgress
            ? client.match
              ? "YOU ARE IN THE MATCH"
              : "MATCH ALREADY IN PROGRESS"
            : "JOIN THE MATCH"}
        </p>
        {client.match && <InGame client={client} />}
      </main>
    </div>
  );
}

type Props = { client: ReturnType<typeof useClient> };
const InGame = ({ client }: Props) => {
  const match = useMatch(client.match, socket, client.host);

  return (
    <div>
      <h2>LETS GO LETS GO</h2>
      {match.tileRequests > 0 && (
        <div>
          <button onClick={() => match.onSelectTrack(1)}>LEFT</button>
          <button onClick={() => match.onSelectTrack(0)}>STRAIGHT</button>
          <button onClick={() => match.onSelectTrack(2)}>RIGHT</button>
        </div>
      )}
    </div>
  );
};

const client = new Client("defaultkey", "localhost", "7350");
const socket = client.createSocket();

const useClient = () => {
  const [session, setSession] = useState<Session>(null);
  const [connected, setIsConnected] = useState(false);
  const [isConnecting, setIsConnecting] = useState(false);
  const [isMatchInProgress, setMatchInProgress] = useState(true);
  const [match, setMatch] = useState<Match | null>(null);
  const [host, setHost] = useState<Presence>();

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
    await socket.addMatchmaker("+properties.type:host", 2, 3);
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
  };
};
