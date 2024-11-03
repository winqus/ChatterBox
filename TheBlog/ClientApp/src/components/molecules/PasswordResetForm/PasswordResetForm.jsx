import { Form } from 'reactstrap';
import FormInputPill from '../../atoms/FormInputPill/FormInputPill';
import FormHeading from '../../atoms/FormHeading/FormHeading';
import FormParagraph from '../../atoms/FormParagraph/FormParagraph';
import ActionButton from '../../atoms/ActionButton/ActionButton';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import ErrorList from '../../atoms/ErrorList/ErrorList';

export default function PasswordResetForm() {
  const location = useLocation();
  const code = (new URLSearchParams(location.search)).get('code');
  const userId = (new URLSearchParams(location.search)).get('userId');
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
    .then(async (data) => {
      setIsLoading(false);
      if (data.ok) {
        const txt = await data.text();
        setErrors([txt]);
        setTimeout(() => navigate('/account/login'), 2000);
      } else {
        setErrors(['Invalid.']);
      }
    })
    .finally(() => {
      setIsLoading(false);
    });
  };

  return (
    <>
      <Form className='passwordForm' action="/api/account/resetpassword" method="post" onSubmit={handleFormSubmit}>
        <FormHeading>Reset Password</FormHeading>
        <FormParagraph>Enter new password for your account.</FormParagraph>
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
        <input id="input_token_1" name="Code" defaultValue={code || '' } className="visually-hidden" />
        <input id="input_token_2" name="UserId" defaultValue={userId || '' } className="visually-hidden" />
        <ErrorList errors={errors} />
        <ActionButton text="Reset Password" loading={isLoading} args={{type: 'submit'}} />
        <FormParagraph>
          Remembered? <Link to="/account/login">Log In</Link>
        </FormParagraph>
        {}
      </Form>
    </>
  );
}
