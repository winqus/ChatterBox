import './NavMenu.scss';
import { useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { Link } from 'react-router-dom';
import UserNav from '../UserNav/UserNav';

export default function NavMenu() {
  const [collapsed, setCollapsed] = useState(true);

  const toggleNavbar = () => setCollapsed(!collapsed);

  return (
    <header>
      <Navbar
        className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3 nav-menu__nav-bar"
        container
        light
        sticky="top"
      >
        <NavbarBrand tag={Link} to="/">The Blog</NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
          <ul className="navbar-nav flex-grow align-items-center">
            <UserNav />
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
}
