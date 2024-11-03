import './MenuButton.scss';
import { Button } from 'reactstrap';

export default function MenuButton({ text, onClick, args }) {
  return (
    <Button
      className="text-black rounded-pill menu__button w-100"
      color="light"
      onClick={onClick}
      {...args}
    >
      {text}
    </Button>
  );
}
