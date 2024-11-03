import './RegisterForm.scss';
import { Form } from 'reactstrap';
import FormInputPill from '../../atoms/FormInputPill/FormInputPill';
import FormHeading from '../../atoms/FormHeading/FormHeading';
import FormParagraph from '../../atoms/FormParagraph/FormParagraph';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { ApplicationPaths } from '../../api-authorization/ApiAuthorizationConstants';
import ErrorList from '../../atoms/ErrorList/ErrorList';
import ActionButton from '../../atoms/ActionButton/ActionButton';
import { Link } from 'react-router-dom';

export default function RegisterForm() {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState([]);

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    const data = new URLSearchParams(new FormData(e.target));
    await fetch(e.target.action, {
      method: "POST",
      body: data,
    })
    .then(async (res) => {
      setIsLoading(false);
      if (res.ok) {
        navigate(ApplicationPaths.Login);
      } else {
        setErrors([`${await res.text()}`]);
      }
    });
  };

  return (
    <Form className='registerForm' action="/api/account/register" method="post" onSubmit={handleFormSubmit}>
      <FormHeading>Sign Up</FormHeading>
      <FormParagraph>Create an account to gain access to more site functionality.</FormParagraph>
      <FormInputPill
        id="username_1"
        name="Username"
        label="Username"
        type="text"
        required
      />
      <FormInputPill
        id="email_1"
        name="Email"
        label="Email"
        type="email"
        required
      />
      <FormInputPill
        id="password_1"
        name="Password"
        label="Password"
        type="password"
        required
      />
      <FormInputPill
        id="password_2"
        name="RepeatPassword"
        label="Repeat Password"
        type="password"
        required
      />
      <ErrorList errors={errors} />
      <ActionButton text="Sign Up" loading={isLoading} args={{type: 'submit'}} />
      <FormParagraph>
        Already a user? <Link to="/account/login">Log in</Link>
      </FormParagraph>
    </Form>
  );
}
