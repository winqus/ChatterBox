import { Container } from 'reactstrap';
import NavMenu from './organisms/NavMenu/NavMenu';

export default function Layout({ children }) {
  return (
    <div>
      <NavMenu />
      <Container tag="main">
        {children}
      </Container>
    </div>
  );
}
