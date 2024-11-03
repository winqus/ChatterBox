import { Link } from "react-router-dom";
import Articles from "../Articles/Articles";

export default function Home() {
  return (
    <>
      <Link to="/article/create">Create article</Link>
      <Articles />
    </>
  );
}
