import Head from "next/head";
import styles from "../styles/Home.module.css";
import { Client } from "@heroiclabs/nakama-js";
import { useEffect, useState } from "react";
import { Session } from "@heroiclabs/nakama-js/dist/session";

export default function Home() {
  const [partyId, setPartyId] = useState<string>();
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
      </main>
    </div>
  );
}

const client = new Client("defaultkey", "192.168.0.100", "7350");
const socket = client.createSocket();

const useClient = () => {
  const [session, setSession] = useState<Session>(null);
  const [connected, setIsConnected] = useState(false);
  const [isConnecting, setIsConnecting] = useState(false);

  useEffect(() => {
    function onDisconnect(e: Event) {
      setIsConnected(false);
    }
    socket.onerror = (error) => {
      console.error(`Received socket error`, error);
    };
    socket.ondisconnect = onDisconnect;
  }, []);

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
      console.log(`JOINED MATCH ${match.match_id}`);
    };

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

  return { connected, session, connect, join };
};
