import './ActionButton.scss';
import { Button, Spinner } from 'reactstrap';

export default function ActionButton({
  text,
  onClick = () => {},
  loading = false,
  args,
}) {
  return (
    <Button
      className={`text-white rounded-pill w-100 menu__submit-button ${ loading ? "menu__submit-button--loading" : undefined}`}
      onClick={onClick}
      disabled={loading}
      {...args}
    >
      {!loading ? text : <Spinner size="sm" />}
    </Button>
  );
}
